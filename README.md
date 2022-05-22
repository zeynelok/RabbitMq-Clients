# RabbitMQ Clients And Redis
This project has RabbitMQ clients and Redis process. 
Producer (Console App) produce a message every 5 second and publish to topic exchange of RabbitMQ. 
Topic exchange forwards the message to the corresponding queue. 
Then if any consumer listen the queue, RabbitMQ sends the message to Consumer (Console App) and when the Consumer receives the message it writes the message to Redis.    

&nbsp;
&nbsp;
&nbsp;

![System schema](https://github.com/zeynelok/RabbitMq-Clients/blob/master/RabbitMQ_Schema.png)
