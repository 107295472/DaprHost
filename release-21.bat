dotnet publish  -c Release --self-contained false
E:
cd E:\doc\linServer\shell
pscp  -pw root123  -r   D:\github\daprserver\Services\AccountService\Host\bin\Release\net6.0\publish\ root@192.168.10.21:/home/data/app/dapr/account 
