using System.Collections;
using System.Diagnostics;
using System.Net.Sockets;
using System.Runtime.InteropServices;

#region Main
Console.WriteLine("Client is Active!");
Console.WriteLine("***************************************************");

int i = 1;
string time = GetInfo()[0].ToString();
string Ip = GetInfo()[1].ToString();
string kaynak = GetInfo()[2].ToString();



while (i > 0)
{
    if (IsConnectedToInternet()) { InternetConnection("release"); }

    if (DateTime.Now.ToString("HH:mm:ss") == time)
    {
        if (!IsConnectedToInternet())
        {
            InternetConnection("renew");
            i = await ClientRun(Ip);
            InternetConnection("release");
        }
        else
        {
            InternetConnection("release");
        }
    }
    i = 1;
}
#endregion

#region IPControl
[DllImport("wininet.dll")]
extern static bool InternetGetConnectedState(out int Description, int ReservedValue);
static bool IsConnectedToInternet()
{
    int Desc;
    return InternetGetConnectedState(out Desc, 0);
}
#endregion

#region Client
async Task<int> ClientRun(string ip)
{
    string[] dosyalar = Directory.GetFiles(@$"{kaynak}");
    try
    {
        foreach (var dosya in dosyalar)
        {
            TcpClient tcpClient = new TcpClient(ip, 1234);
            Console.WriteLine("Connected. Sending file.");

            StreamWriter sWriter = new StreamWriter(tcpClient.GetStream());
            byte[] bytes = await System.IO.File.ReadAllBytesAsync(dosya);

            await sWriter.WriteLineAsync(dosya);
            sWriter.Flush();

            FileInfo fi = new FileInfo(dosya);

            Console.WriteLine($"Sending file is {dosya}");
            await tcpClient.Client.SendFileAsync(dosya);

            Thread.Sleep(1000);
        }
    }
    catch (Exception e) { Console.Write(e.Message); }
    Console.WriteLine("Is Completed!");
    return 0;
}
#endregion

#region IP_Open_or_Close
void InternetConnection(string str)
{

    Process p = new Process();
    p.StartInfo.UseShellExecute = false;
    p.StartInfo.RedirectStandardOutput = true;
    p.StartInfo.FileName = "cmd.exe";
    p.StartInfo.Arguments = "/C ipconfig /" + str;
    p.Start();

    if (str == "release")
    {
        Console.BackgroundColor = ConsoleColor.Red;
        Console.WriteLine($"Connection is Closed");
    }
    else
    {
        Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine($"Connection is Opened");
    }
    Console.BackgroundColor = ConsoleColor.Black;
    Thread.Sleep(10000);
}
#endregion

#region TxtInfo
ArrayList GetInfo()
{
    var arr = new ArrayList();
    using (StreamReader sr = new StreamReader("benioku.txt"))
    {
        string satir;
        while ((satir = sr.ReadLine()) != null)
        {
            arr.Add(satir);
        }
    }
    return arr;
}
#endregion