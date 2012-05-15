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


        protected void Page_Init(object sender, EventArgs e)
        {
                
        }


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
                btnCancel.Click += new System.EventHandler(btnCancel_Click);
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

        private void AddToRecieverCollection(ArrayList objRecieverCollection, MethodInfo[] objMethodsInfo)
        {
            foreach (MethodInfo objMethodInfo in objMethodsInfo)
            {
                objRecieverCollection.Add(objMethodInfo.Name);
            }
        }

        void btnCancel_Click(object sender, EventArgs e)
        {

            this.Page.RegisterClientScriptBlock("Close", "<script>javascript:window.close();</script>");
        }

        void btnRegister_Click(object sender, EventArgs e)
        {



            string AssemblyInfo;
            SPEventReceiverDefinitionCollection eventReceivers;
            Guid objSelectedRowID;
            try
            {               
                
                // our code should be inserted here
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    SPWeb ospWeb = SPContext.Current.Web;
                    Microsoft.SharePoint.Administration.SPWebApplication webApp = ospWeb.Site.WebApplication;
                    webApp.FormDigestSettings.Enabled = false;

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


                    this.Page.RegisterClientScriptBlock("CloseForm", "<script>javascript:window.close();window.opener.location.reload();</script>");
                    webApp.FormDigestSettings.Enabled = true;


                });     

                
            }
            catch (Exception ex)
            {
                string strError = ex.Message.Replace("'", " ");
                Response.Write(strError);
            }
        }
    }

}

