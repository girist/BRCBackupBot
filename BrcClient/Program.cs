// See https://aka.ms/new-console-template for more information
using System.Collections;
using System.Diagnostics;
using System.Net.Sockets;
using System.Runtime.InteropServices;


#region Main
Console.WriteLine("Client is Active!");
int i = 1;
string time = GetInfo()[0].ToString();
string Ip = GetInfo()[1].ToString();

while (i > 0)
{
    if (DateTime.Now.ToString("HH:mm:ss") == time)
    {
        if (!IsConnectedToInternet()) { InternetConnection("renew"); }
        Thread.Sleep(10000);
        i = await ClientRun(Ip);
        Thread.Sleep(10000);
        if (IsConnectedToInternet()) { InternetConnection("release"); }
        Console.WriteLine($"Connection Status :{IsConnectedToInternet()}");

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
    string[] dosyalar = Directory.GetFiles(@"C:\Users\ChiLL\OneDrive\Masaüstü\kaynak");
    try
    {
        foreach (var dosya in dosyalar)
        {
            TcpClient tcpClient = new TcpClient(ip, 1234);
            Console.WriteLine("Connected. Sending file.");

            StreamWriter sWriter = new StreamWriter(tcpClient.GetStream());
            byte[] bytes = await System.IO.File.ReadAllBytesAsync(dosya);

            await sWriter.WriteLineAsync(bytes.Length.ToString());
            sWriter.Flush();

            await sWriter.WriteLineAsync(dosya);
            sWriter.Flush();

            Console.WriteLine($"Sending file is {dosya}");
            await tcpClient.Client.SendFileAsync(dosya);
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
    ProcessStartInfo internet = new ProcessStartInfo()
    {
        FileName = "cmd.exe",
        Arguments = "/C ipconfig /" + str
    };
    Process.Start(internet);
} 
#endregion

ArrayList GetInfo()
{
    var arr = new ArrayList();
    using (StreamReader sr = new StreamReader("benioku.txt")) //StreamReader fonksiyonu ile okunacak dosyamızı açtırıyoruz.
    {
        string satir; //burada okuduğunuz her satırı atamamız için gerekli değişkeni tanımlıyoruz.
        while ((satir = sr.ReadLine()) != null) //Döngü kurup eğer satır boş değilse, satirlarList List'ine ekleme yapıyoruz.
        {
            arr.Add(satir);
        }
    }
    return arr;
}