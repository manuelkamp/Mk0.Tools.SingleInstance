# Mk0.Tools.SingleInstance
(C) 2019 mk0.at

With this code you are able to make your application a single instance.

Usage example:

static void Main(string[] args)
{
	Application.EnableVisualStyles();
	Application.SetCompatibleTextRenderingDefault(false);
	SingleApplication.Run(new MainForm(), Demo.Properties.Settings.Default.singleInstance);
}

MainForm is the form to show on start
singleInstance is an optional boolean value, where you can enable/disable SingleInstance within your application settings