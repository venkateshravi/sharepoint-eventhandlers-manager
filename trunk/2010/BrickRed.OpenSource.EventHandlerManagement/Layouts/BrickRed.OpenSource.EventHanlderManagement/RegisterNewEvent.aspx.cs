/*
 ===========================================================================
 Copyright (c) 2010 BrickRed Technologies Limited

 Permission is hereby granted, free of charge, to any person obtaining a copy
 of this software and associated documentation files (the "Software"), to deal
 in the Software without restriction, including without limitation the rights
 to use, copy, modify, merge, publish, distribute, sub-license, and/or sell
 copies of the Software, and to permit persons to whom the Software is
 furnished to do so, subject to the following conditions:

 The above copyright notice and this permission notice shall be included in
 all copies or substantial portions of the Software.

 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 THE SOFTWARE.
 ===========================================================================
 */

using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.Collections;
using Microsoft.SharePoint.WebControls;
using System.Data;
using System.Reflection;
using System.Text;
using Microsoft.SharePoint.Workflow;

namespace BrickRed.OpenSource.EventHandlerManagement
{
    public partial class RegisterNewEvent : LayoutsPageBase
    {
        SPWeb objSPWeb;
        SPList objSPList;
        ArrayList objReceiverListAll;
        protected Button btAddNewEvent;
        string listId = string.Empty;
        String strEventType;
        private ArrayList objRecieverMethodCollection;
        private ArrayList objEventRecieverColection;
        string strActionType = string.Empty;


        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            Table objTable = new Table();

            string strEventHandler = string.Empty;

            try
            {
                listId = Convert.ToString(Page.Request.QueryString[GlobalConstants.LISTID]);
                strActionType = Convert.ToString(Page.Request.QueryString[GlobalConstants.ACTIONTYPE]);
                if (String.IsNullOrEmpty(listId))
                {
                    strEventType = GlobalConstants.WEB_EVENTTYPE;
                    objSPWeb = SPContext.Current.Web;
                }
                else
                {
                    strEventType = GlobalConstants.LIST_EVENTTYPE;
                    objSPList = SPContext.Current.Web.Lists[new Guid(listId)];
                }

                btnRegister.Click += new System.EventHandler(btnRegister_Click);

                if (!this.Page.IsPostBack)
                {
                    objSPWeb = SPContext.Current.Web;
                    objReceiverListAll = new ArrayList();
                    objRecieverMethodCollection = new ArrayList();
                    switch (strEventType)
                    {
                        case GlobalConstants.LIST_EVENTTYPE:
                            objRecieverMethodCollection = new ArrayList();
                            MethodInfo[] objMethodsInfo = typeof(SPItemEventReceiver).GetMethods();
                            AddToRecieverCollection(objRecieverMethodCollection, objMethodsInfo);
                            objMethodsInfo = typeof(SPEmailEventReceiver).GetMethods();
                            AddToRecieverCollection(objRecieverMethodCollection, objMethodsInfo);
                            objMethodsInfo = typeof(SPWorkflowEventReceiver).GetMethods();
                            AddToRecieverCollection(objRecieverMethodCollection, objMethodsInfo);
                            break;
                        case GlobalConstants.WEB_EVENTTYPE:
                            objRecieverMethodCollection = new ArrayList();
                            objMethodsInfo = typeof(SPListEventReceiver).GetMethods();
                            AddToRecieverCollection(objRecieverMethodCollection, objMethodsInfo);
                            objMethodsInfo = typeof(SPWebEventReceiver).GetMethods();
                            AddToRecieverCollection(objRecieverMethodCollection, objMethodsInfo);
                            break;

                    }
                    foreach (SPEventReceiverType objRecievertype in Enum.GetValues(typeof(SPEventReceiverType)))
                    {
                        objReceiverListAll.Add(objRecievertype.ToString());
                    }

                    objEventRecieverColection = new ArrayList();
                    foreach (var Item in objRecieverMethodCollection)
                    {
                        if (objReceiverListAll.Contains(Item))
                        {
                            objEventRecieverColection.Add(Item);
                        }
                    }

                    ddlInActiveEvents.DataSource = objEventRecieverColection;
                    ddlInActiveEvents.DataBind();
                    if (strActionType == GlobalConstants.ACTIONTYPE_EDIT_CLICK)
                    {
                        strEventHandler = Convert.ToString(Page.Request.QueryString[GlobalConstants.Query_String_EVENT_TYPE]);
                        ddlInActiveEvents.SelectedIndex = ddlInActiveEvents.Items.IndexOf(((ListItem)ddlInActiveEvents.Items.FindByText(strEventHandler)));
                        txtClassName.Text = Convert.ToString(Page.Request.QueryString[GlobalConstants.Query_String_CLASS]);
                        txtCulture.Text = Convert.ToString(Page.Request.QueryString[GlobalConstants.Query_String_CULTURE]);
                        txtPublicKeyToken.Text = Convert.ToString(Page.Request.QueryString[GlobalConstants.QUERY_STRING_CULTURE]);
                        txtVersion.Text = Convert.ToString(Page.Request.QueryString[GlobalConstants.QUERY_STRING_VERSION]);
                        txtAssemblyName.Text = Convert.ToString(Page.Request.QueryString[GlobalConstants.QUERY_STRING_ASSEMBLY_NAME]);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion Page_Load

        #region AddMethodToRecieverCollection

        private void AddToRecieverCollection(ArrayList objRecieverCollection, MethodInfo[] objMethodsInfo)
        {
            foreach (MethodInfo objMethodInfo in objMethodsInfo)
            {
                objRecieverCollection.Add(objMethodInfo.Name);
            }
        }

        #endregion AddMethodToRecieverCollection

        void btnCancel_Click(object sender, EventArgs e)
        {


        }

        #region RegisterEvents & EditEvents

        void btnRegister_Click(object sender, EventArgs e)
        {
            string strFormatString;
            string strJavaScript;
            string AssemblyInfo;
            SPEventReceiverDefinitionCollection eventReceivers;
             Guid objSelectedRowID;
            try
            {
                SPEventReceiverType objType = (SPEventReceiverType)Enum.Parse(typeof(SPEventReceiverType), ddlInActiveEvents.SelectedValue);


                AssemblyInfo = txtAssemblyName.Text + "," + GlobalConstants.VERSION + txtVersion.Text + "," + GlobalConstants.CULTURE + txtCulture.Text + "," + GlobalConstants.PUBLICKEYTOKEN + txtPublicKeyToken.Text;

                //code to register the event
                if (strEventType == GlobalConstants.LIST_EVENTTYPE)
                {
                    objSPList = SPContext.Current.Web.Lists[new Guid(listId)];
                    eventReceivers = objSPList.EventReceivers;
                    objSPList.EventReceivers.Add(objType, AssemblyInfo, txtClassName.Text);
                    objSPList.Update();
                }
                else
                {
                    eventReceivers = SPContext.Current.Web.EventReceivers;
                    SPContext.Current.Web.EventReceivers.Add(objType, AssemblyInfo, txtClassName.Text);
                    SPContext.Current.Web.Update();
                }

                if (strActionType == GlobalConstants.ACTIONTYPE_EDIT_CLICK)
                {

                    //Label lblGUID = (Label)objGridViewRow.Cells[5].FindControl("lblGUID");

                    //objSelectedRowID = new Guid(lblGUID.Text);
                    objSelectedRowID = new Guid(Convert.ToString(Page.Request.QueryString["GUID"]));

                    eventReceivers[objSelectedRowID].Delete();
                    
                    
                    //foreach (SPEventReceiverDefinition def in eventReceivers)
                    //{

                    //    if (Convert.ToString(def.Type) == Convert.ToString(Page.Request.QueryString[GlobalConstants.Query_String_EVENT_TYPE]) && Convert.ToString(def.Class) == Convert.ToString(Page.Request.QueryString[GlobalConstants.Query_String_CLASS]))
                    //    {
                    //        def.Delete();
                    //        break;
                    //    }

                    //}
                }
                
                btnRegister.Attributes.Add("OnClientClick", "javascript:SP.UI.ModalDialog.commonModalDialogClose(SP.UI.DialogResult.OK, null);return false;");
                this.Context.Response.Write("<script type='text/javascript'>window.frameElement.commitPopup();</script>");
                this.Context.Response.Flush();
                this.Context.Response.End();


            }
            catch (Exception ex)
            {
                string objErrorMsg = ex.Message.Replace("'", " ");
                strFormatString = string.Format("javascript:ShowFailureStatusBarMessage('{0}','{1}');", "Error :", objErrorMsg);
                strJavaScript = "<script type=\"text/javascript\">ExecuteOrDelayUntilScriptLoaded(Initialize, \"sp.js\");function Initialize(){ " + strFormatString + " }</script>";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "opennotification", strJavaScript, false);
            }
        }

        #endregion RegisterEvents & EditEvents
    }

}

