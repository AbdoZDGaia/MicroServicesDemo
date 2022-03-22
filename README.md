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
  - Run the command `kubectl apply -f local-pvc.yaml`
  - Run the command `kubectl apply -f mssql-plat-depl.yaml`
Enjoy.
