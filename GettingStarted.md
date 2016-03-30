## Prerequisites ##
  1. SharePoint Foundation 2010 / SharePoint 2010 Environment
## Step 1. WSP Installation ##
  1. Download [WSP for 2010](http://sharepoint-eventhandlers-manager.googlecode.com/files/sharepoint-eventhandlers-manager-2010-2.0.zip) and unzip the Event handler management archive.
> This contains the WSP file, deploy solution powershell script file and retract solution powershell script file
  1. Right click and select "Run with PowerShell" comand to deploy the solution file <br /> ![http://sharepoint-eventhandlers-manager.googlecode.com/svn/wiki/Images/run-with-powershell.png](http://sharepoint-eventhandlers-manager.googlecode.com/svn/wiki/Images/run-with-powershell.png)
  1. It will ask you for the web URL and will install the solution package on your server <br /> ![http://sharepoint-eventhandlers-manager.googlecode.com/svn/wiki/Images/install-solution.png](http://sharepoint-eventhandlers-manager.googlecode.com/svn/wiki/Images/install-solution.png)
  1. This will add the solution in solutions gallery <br /> http://sharepoint-eventhandlers-manager.googlecode.com/svn/wiki/Images/solution-properties.PNG
  1. This will also deploy two features in site collection features gallery <br /> http://sharepoint-eventhandlers-manager.googlecode.com/svn/wiki/Images/write-features-gallery.PNG

## Step 2. Functionality Overview ##
  1. Go to Site Settings then Event Handlers Settings OR go to any list settings and then Event Handlers Settings <br /> ![http://sharepoint-eventhandlers-manager.googlecode.com/svn/wiki/Images/event-handler-settings.png](http://sharepoint-eventhandlers-manager.googlecode.com/svn/wiki/Images/event-handler-settings.png)
  1. Here you will see the existing event handlers attached to web or list. <br /> ![http://sharepoint-eventhandlers-manager.googlecode.com/svn/wiki/Images/event-handler-list.png](http://sharepoint-eventhandlers-manager.googlecode.com/svn/wiki/Images/event-handler-list.png)
  1. You can register new event handler by clicking "Register New Event Handler" link. Make sure that you already placed the DLL in GAC before this step otherwise system will throw an error <br /> ![http://sharepoint-eventhandlers-manager.googlecode.com/svn/wiki/Images/event-handler-register.png](http://sharepoint-eventhandlers-manager.googlecode.com/svn/wiki/Images/event-handler-register.png)
> Thats all you need to manage your event handlers in SharePoint 2010 environment and you dont require to register and unregister them from code anymore.