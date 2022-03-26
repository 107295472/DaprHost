# DaprServer

##### 原项目来自大神下岗的老王  https://github.com/sd797994/Oxygen-Dapr.EshopSample 

####  本地环境
安装tye,可到nuget.org查看最新版本 <br/>
dotnet tool install --global Microsoft.Tye --version 0.11.0-alpha.22111.1<br/>
在tye.yaml配置后运行run.bat启动 ，如需断点，附加进程调试 <br/>

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

