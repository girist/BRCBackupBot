// See https://aka.ms/new-console-template for more information
using System.Net;
using System.Net.Sockets;

Console.WriteLine("Server is Active!");

TcpListener tcpListener = new TcpListener(IPAddress.Any, 1234);
tcpListener.Start();

Console.WriteLine("Server started");

while (true)
{
    try
    {
        Console.WriteLine("***************************");
        TcpClient tcpClient = tcpListener.AcceptTcpClient();

        Console.WriteLine("Connected to client");

        StreamReader reader = new StreamReader(tcpClient.GetStream());
        string cmdFileSize = reader.ReadLine();
        string cmdFileName = reader.ReadLine();
        Console.WriteLine(cmdFileName);

        int length = Convert.ToInt32(cmdFileSize);
        byte[] buffer = new byte[length];
        int received = 0;
        int read = 0;
        int size = 1024;
        int remaining = 0;
        
        while (received < length)
        {
            remaining = length - received;
            if (remaining < size)
            {
                size = remaining;
            }

            read = tcpClient.GetStream().Read(buffer, received, size);
            received += read;
        }
        using (FileStream fStream = new FileStream(Path.GetFileName(cmdFileName), FileMode.Create))
        {
            Console.WriteLine(cmdFileName);
            fStream.Write(buffer, 0, buffer.Length);
            fStream.Flush();
            fStream.Close();
        }
        Console.WriteLine("File received and saved in " + Environment.CurrentDirectory);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message.ToString());
    }
}
