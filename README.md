# DaprServer

####  本地环境
安装tye,可到nuget.org查看最新版本 <br/>
dotnet tool install --global Microsoft.Tye --version 0.11.0-alpha.22111.1<br/>
在tye.yaml配置后运行run.bat启动 ，如需断点，附加进程调试 <br/>
mysql请自行安装,数据库名admindb

#### 配置中心
```
docker run -d --name myconfig --restart=always -e adminConsole=true -e db:provider=mysql -e db:conn="Server=host.docker.internal;Database=configcenter;Uid=root;Pwd=root123;Port=3306;charset=utf8" -p 4999:5000 -v /etc/localtime:/etc/localtime kklldog/agile_config:latest

初始化json,SyncData和SyncStructure第一次true
第一次运行ApplicationService/Host启动初始化数据，之后SyncData和SyncStructure改为false
{
  "AppConfig": {
    "CacheType": "1",
    "Tenant": "true"
  },
  "DbConfig": {
    "ConnectionString": "Data Source=localhost;Port=3306;User ID=root;Password=root123; Initial Catalog=admindb;Charset=utf8; SslMode=none;Min pool size=1",
    "Curd": "true",
    "DataType": "0",
    "MonitorCommand": "false",
    "RedisConnStr": "localhost:6379,defaultDatabase=14",
    "SyncData": "true",
    "SyncStructure": "true"
  }
}
```



#### 生产环境
docker环境运行<br/>
运行release-local.bat发布程序到bin\Release\net6.0\publishl目录，并且复制config文件夹到publishl目录<br/>
编辑docker-compose.yaml配置要运行的程序<br/>
运行命令docker-compose  up -d  打包并启动程序

#### vs2022 扩展
Open Command Line 命令行，运行项目里的bat

#### dapr 安装   前提要安装docker

下载dapr cli https://github.com/dapr/cli/releases
加入到系统环境变量里

```
执行dapr init  安装失败，请到 https://github.com/dapr 手动下载
```

