using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.Collections;
using Microsoft.SharePoint.WebControls;
using System.Data;
using System.Text;
using System.Collections.Generic;

namespace BrickRed.OpenSource.EventHandlerManagement
{
    public partial class ConfigurationWizard : LayoutsPageBase
    {
        SPWeb objSPWeb;
        SPList objSPList;
        SPEventReceiverDefinitionCollection objEventReceiversCollection;
        protected LinkButton btAddNewEvent;
        static string listId = string.Empty;


        protected void Page_Load(object sender, EventArgs e)
        {
            string url;
            try
            {
                objSPWeb = SPContext.Current.Web;
                listId = Page.Request.QueryString[GlobalConstants.LISTID];
                if (listId != null)
                {
                    objSPList = SPContext.Current.Web.Lists[new Guid(listId)];
                    objEventReceiversCollection = objSPList.EventReceivers;
                }
                else
                {
                    objEventReceiversCollection = SPContext.Current.Web.EventReceivers;
                }

                BindGrid(objEventReceiversCollection);
                                
                url = SPContext.Current.Web.Url + "/_layouts/BrickRed.OpenSource.EventHandlerManagement/RegisterNewEvent.aspx?ListId=" + listId;
                btAddNewEvent.Attributes.Add("onclick", "Javascript:window.open('" + url + "','','menubar=no,height=400,width=650,toolbar=no,location=no,status=no');return false;");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BindGrid(SPEventReceiverDefinitionCollection objEventReceiversCollection)
        {
            try
            {

                EventHandler objEventHandler;
                List<EventHandler> objEventHandlerList = new List<EventHandler>();
                if (objEventReceiversCollection.Count > 0)
                {
                    foreach (SPEventReceiverDefinition def in objEventReceiversCollection)
                    {
                        objEventHandler = new EventHandler();
                        objEventHandler.EVENTTYPE = def.Type.ToString();
                        objEventHandler.ASSEMBLYINFO = def.Assembly;
                        objEventHandler.CLASSNAME = def.Class;
                        objEventHandler.DEFID = def.Id;
                        objEventHandlerList.Add(objEventHandler);
                    }
                    objGridView.DataSource = null;
                    objGridView.DataBind();
                    objGridView.DataSource = objEventHandlerList;
                    objGridView.DataBind();
                    lblNoEventRegisteres.Visible = false;
                }
                else
                {
                    objEventHandler = new EventHandler();
                    objEventHandlerList.Add(objEventHandler);
                    objGridView.DataSource = objEventHandlerList;
                    objGridView.DataBind();
                    objGridView.Rows[0].Visible = false;
                    lblNoEventRegisteres.Visible = true;
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                string strEditPageUrl;
                ImageButton objImageButton;
                ImageButton objImgEditBtn;
                string[] strAssemblyInfo;
                string strVersion = string.Empty;
                string strPublicKeyToken = string.Empty;
                string strCulture = string.Empty;
                string strClassName = string.Empty;
                Label lblGUID;
                Guid objGUID;                

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (e.Row.Cells[4].FindControl(GlobalConstants.BTN_DELETE) != null)
                    {
                        objImageButton = (ImageButton)e.Row.Cells[4].FindControl(GlobalConstants.BTN_DELETE);
                        objImageButton.Attributes.Add("onclick", "Javascript:return confirm('Are you sure, you want to delete this handler?');");
                        
                    }
                    if (e.Row.Cells[3].FindControl(GlobalConstants.BTN_EDIT) != null)
                    {
                        //Get assembly info start//
                        if (!(string.IsNullOrEmpty(e.Row.Cells[1].Text)) && e.Row.Cells[1].Text != GlobalConstants.SPACE)
                        {
                            strAssemblyInfo = e.Row.Cells[1].Text.Split(',');
                            strVersion = strAssemblyInfo[1].Substring(strAssemblyInfo[1].IndexOf("=") + 1, strAssemblyInfo[1].Length - strAssemblyInfo[1].IndexOf("=") - 1);
                            strCulture = strAssemblyInfo[2].Substring(strAssemblyInfo[2].IndexOf("=") + 1, strAssemblyInfo[2].Length - strAssemblyInfo[2].IndexOf("=") - 1);
                            strPublicKeyToken = strAssemblyInfo[3].Substring(strAssemblyInfo[3].IndexOf("=") + 1, strAssemblyInfo[3].Length - strAssemblyInfo[3].IndexOf("=") - 1);
                            strClassName = e.Row.Cells[2].Text;

                            lblGUID = (Label)e.Row.Cells[5].FindControl(GlobalConstants.LBLGUID);
                            objGUID = new Guid(lblGUID.Text);

                            //Get assembly info End//
                            objImgEditBtn = (ImageButton)e.Row.Cells[3].FindControl(GlobalConstants.BTN_EDIT);

                            strEditPageUrl = SPContext.Current.Web.Url + "/_layouts/BrickRed.OpenSource.EventHandlerManagement/RegisterNewEvent.aspx?ListId=" + listId + "&ActionType=Edit_Click&EventType=" + e.Row.Cells[0].Text + "&Class=" + strClassName + "&Culture=" + strCulture + "&PublicKey=" + strPublicKeyToken + "&Version=" + strVersion + "&AssemblyName=" + strAssemblyInfo[0] + "&GUID=" + objGUID; 
                            objImgEditBtn.Attributes.Add("onclick", "Javascript:window.open('" + strEditPageUrl + "','','menubar=no,height=300,width=650,toolbar=no,location=no,status=no');return false;");
                        }
                    }
                }
             
                
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void Edit_Click(object sender, EventArgs e)
        {
        }

        protected void Delete_Click(object sender, EventArgs e)
        {
            try
            {
                DeleteEventHandler(sender);                
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        private void DeleteEventHandler(object sender)
        {
            GridViewRow objGridViewRow;
            GridView objGridView;
            SPList objSPList;
            int objSelectedRowIndex;
            string objSelectedRow = string.Empty;
            Guid objSelectedRowID;
            SPEventReceiverDefinitionCollection eventReceivers;
            if (listId != null)
            {
                objSPList = SPContext.Current.Web.Lists[new Guid(listId)];
                eventReceivers = objSPList.EventReceivers;
            }
            else
            {
                eventReceivers = SPContext.Current.Web.EventReceivers;
            }

            System.Web.UI.WebControls.ImageButton objImage = (System.Web.UI.WebControls.ImageButton)sender;
            objGridViewRow = (GridViewRow)objImage.NamingContainer;
            objGridView = (GridView)objGridViewRow.Parent.NamingContainer;

            objSelectedRowIndex = objGridViewRow.RowIndex;

            Label lblGUID = (Label)objGridViewRow.Cells[5].FindControl(GlobalConstants.LBLGUID);
            objSelectedRowID = new Guid(lblGUID.Text);
            eventReceivers[objSelectedRowID].Delete();
                       
            if (listId != null)
            {
                objSPList = SPContext.Current.Web.Lists[new Guid(listId)];
                BindGrid(objSPList.EventReceivers);
            }
            else
            {
                using (SPSite objSPsite = new SPSite(SPContext.Current.Site.Url))
                {
                    using (SPWeb objWeb = objSPsite.OpenWeb())
                    {
                        eventReceivers = objWeb.EventReceivers;
                    }
                }                       

                BindGrid(eventReceivers);
            }
        }

            

    }
}
