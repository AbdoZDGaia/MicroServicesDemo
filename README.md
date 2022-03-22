# MicroServicesDemo
A medium scale solution built using the Microservices architecture (gRpc, REST, RabbitMQ, .Net 6, SQL) and (ingress-nginx) for Load balancing
##### Docker and Kubernetes are used for containerization and orchestration

In order to actually run it:
- Download Docker Desktop
- Enable Kubernetes (since it's not enabled by default)
- Create an account on Docker Hub
- Go to K8S folder and press Shift + Right click then choose (Open powershell window here) or alternatively open cmd and navigate to K8S folder
  - Run the command `docker build -t (your dockerhub id)/platformservice .`
  - Run the command `docker push (your dockerhub id)/platformservice`
  - Run the command `docker build -t (your dockerhub id)/commandservice .`
  - Run the command `docker push (your dockerhub id)/commandservice`
  - Run the command `kubectl apply -f platforms-depl.yaml`
  - Run the command `kubectl apply -f platforms-np-srv.yaml`
  - Run the command `kubectl apply -f commands-depl.yaml`
  - Go check out the command below [ingress-nginx Docker Desktop Installation](https://kubernetes.github.io/ingress-nginx/deploy/#quick-start) which should be something like the following
    - Run the command `kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.1.2/deploy/static/provider/cloud/deploy.yaml`
  - Run the command `kubectl apply -f ingress-srv.yaml`
    - Within the ingress-srv.yaml you should find a `abdoz.org`, feel free to change it or keep it as is
    - Open up hosts within `C:\Windows\System32\drivers\etc`
    - Add the following line `127.0.0.1 abdoz.org` and in case you have changed `abdoz.org` within ingress-srv.yaml, change it here accordingly as well
    - If you run the command `kubectl get services --namespace=ingress-nginx` you should get a list of the nginx services (load balancer & cluster IP)
  - Run the command `kubectl apply -f local-pvc.yaml`
  - Run the command `kubectl create secret generic mssql --from-literal=SA_PASSWORD="Some password"` bear in mind that "mssql" is the secret's name, and the "SA_PASSWORD" is actually the secret's key
  - Run the command `kubectl apply -f mssql-plat-depl.yaml`
  - Now give it some time till everything is ready (5 minutes or so)
  - Run the command `kubectl get services` and you should find a `platformservice-srv   NodePort    Some Cluster-IP   Some External-IP    PORT`
  - It should be something similar to this `80:"some port"/TCP` go ahead and copy that port
  - Open up a new chrome tab and enter `http://localhost:"Port"/swagger` and now you can actually explore the platforms service's swagger UI
  - Open up SSMS (SQL Server Management Studio) and connect to `localhost, 1433` and select "SQL Server Authentication" 
  - Enter "sa" as login and enter the password you have picked "Some password" as password
  - Now you are within your created SQL container through the custom load balancer you have created
    - Notice that in case you have created a new DB and went ahead to delete the container from Docker Desktop, the DB still persists. And that's why we have deployed the local-pvc which serves as a "Persistent volume claim"

Enjoy.
