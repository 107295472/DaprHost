# DaprServer

##### 修改自  https://github.com/sd797994/Oxygen-Dapr.EshopSample 

####  1.本地环境
安装tye,可到nuget.org查看最新版本 <br/>
dotnet tool install --global Microsoft.Tye --version 0.11.0-alpha.22111.1<br/>
在tye.yaml配置后运行run.bat启动 ，如需断点，附加进程调试 <br/>

#### 2.生产环境
docker环境运行<br/>
运行release-local.bat发布程序到bin\Release\net6.0\publishl目录，并且复制config文件夹到publishl目录<br/>
编辑docker-compose.yaml配置要运行的程序<br/>
运行命令docker-compose  up -d  打包并启动程序
