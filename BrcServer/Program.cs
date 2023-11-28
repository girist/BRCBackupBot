using System.Collections;
using System.Net;
using System.Net.Sockets;

Console.WriteLine("Server is Active!");
string hedef = GetInfo()[0].ToString();

TcpListener tcpListener = new TcpListener(IPAddress.Any, 1234);
tcpListener.Start();

Console.WriteLine("Server started");


while (true)
{
    Receive();
}

#region Receive
async void Receive()
{
    try
    {
        TcpClient tcpClient = tcpListener.AcceptTcpClient();

        Console.WriteLine("Connected to client");
        Console.WriteLine("**********************************************");

        StreamReader reader = new StreamReader(tcpClient.GetStream());
        string fileName = reader.ReadLine();
        FileInfo fileInfo = new FileInfo(fileName);

        byte[] imgbyte =await File.ReadAllBytesAsync(fileName);

        File.WriteAllBytes($"{hedef}" + $@"\{fileInfo.Name}", imgbyte);
        Console.WriteLine($"{fileInfo.Name}");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message.ToString());
    }
    await Console.Out.WriteLineAsync(DateTime.Now.ToString());
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