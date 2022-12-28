using ConsoleApp2;
using MauiApp2.WinUI;

namespace MauiApp2;


public partial class MainPage : ContentPage
{
	//int count = 0;

	public MainPage()
	{
		InitializeComponent();
	}

	private void OnRunClicked(object sender, EventArgs e)
	{
		string username = UsernameEntry.Text;
		string password = PasswordEntry.Text;
		bool isCheckedPath = ByPathFilter.IsChecked;
		bool isCheckedAll = AllVPGFilter.IsChecked;
		bool isCheckedZORG = ByZorgFilter.IsChecked;
		string pathToFile = PathToFile.Text;
		string zorgID = ZorgID.Text;
		Runner.CreateFile();
        Console.WriteLine();
	}
    private void UsernameEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
		Console.WriteLine("Username changed!");
    }
	private void OnUsernameEntryCompleted(object sender, EventArgs e)
	{
        string text = ((Entry)sender).Text;
		Console.WriteLine(text);
    }
    private void OnPasswordEntryCompleted(object sender, EventArgs e)
    {
        string text = ((Entry)sender).Text;
        Console.WriteLine(text);
    }
}

