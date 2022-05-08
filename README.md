# How it works?
```Agent.exe``` - microservice which collects system metrics and safes them into data base. It creates local server where you can see, delete, update or create them. Which metrics does it collect? These:

- Cpu
- Hdd
- Ram
- Network
- Dotnet

```Client.exe``` - desktop app based on wpf technology. It makes request every 1.2 second to the local server and displays information graphically.


![](https://github.com/ddoo5/Metrics-Client/blob/vid/video/exampleofwork.png)


# How to launch?
You should have downloaded [.Net 6 Framework](https://dotnet.microsoft.com/en-us/download/dotnet/6.0).

Than you download project from [releases](https://github.com/ddoo5/Metrics-Client/releases), unpack it and lauch Agent.exe. After the connection was created you launch Client.exe


![](https://github.com/ddoo5/Metrics-Client/blob/vid/video/how%20to%20launch.png)

# Web usage
You should launch *Agent.exe*. Open your browser. Enter: 

```http://localhost:5000/```

After last / you can choose request and enter it like: 

```http://localhost:5000/api/metric/cpu/all```

Or

```http://localhost:5000/api/metric/cpu/getbyid?5```

Commands are described in more detail in [controllers](https://github.com/ddoo5/Metrics-Client/tree/main/Metrics%20Client%20Code/Agent/Controllers)

**Addresses:**


![](https://github.com/ddoo5/Metrics-Client/blob/vid/video/cpuexample.png)
![](https://github.com/ddoo5/Metrics-Client/blob/vid/video/dotnetexample.png)
![](https://github.com/ddoo5/Metrics-Client/blob/vid/video/hddexample.png)
![](https://github.com/ddoo5/Metrics-Client/blob/vid/video/networkexample.png)
![](https://github.com/ddoo5/Metrics-Client/blob/vid/video/ramexample.png)
