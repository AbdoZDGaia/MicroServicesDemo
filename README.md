# MicroServicesDemo
A medium scale solution built using the Microservices architecture (gRpc, REST, RabbitMQ, .Net 6, SQL)
##### Docker and Kubernetes are used for containerization and orchestration

In order to actually run it:
- Download Docker Desktop
- Enable Kubernetes (since it's not enabled by default)
- Create an account on Docker Hub
- Go to K8S folder and press Shift + Right click then choose (Open powershell window here) or alternatively open cmd and navigate to K8S folder
  - run the command `docker build -t (your dockerhub id)/platformservice .`
  - run the command `docker push (your dockerhub id)/platformservice`
  - run the command `docker build -t (your dockerhub id)/commandservice .`
  - run the command `docker push (your dockerhub id)/commandservice`
  - run the command `kubectl apply -f platforms-depl.yaml`
  - run the command `kubectl apply -f platforms-np-srv.yaml`
  - run the command `kubectl apply -f commands-depl.yaml`
  - run the command `kubectl apply -f ingress-srv.yaml`
  - run the command `kubectl apply -f local-pvc.yaml`
  - run the command `kubectl apply -f mssql-plat-depl.yaml`

Enjoy.
