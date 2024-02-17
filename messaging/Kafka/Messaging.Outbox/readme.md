# Messaging Outbox
## Introduction
This project builds upon Messaging.Example adding the following;
* Groundwork for preparing to move messaging into a library
* The [outbox pattern](https://microservices.io/patterns/data/transactional-outbox.html)

## Setup
Follow the setup instructions from the readme in Messaging.Example and in addition;
1. Create three new topics;
    1. "Outbox-HelloAll"
    2. "Outbox-HelloConsumer1"
    3. "Outbox-HelloConsumer2" 
2. Run the following commands (yaml files found in sandbox\messaging\Kafka\Messaging.Outbox\Messaging.Outbox.Database) in a command prompt;
    ```
    docker-compose -f docker-compose.database.yaml up
    ```
    ```
    docker-compose -f docker-compose.liquibase-update.yaml up
    ```
3. This should create the outbox database and table for you in SQL, use your preferred IDE to check the database table and database both exist.

## Solution structure
The solution consists of three runnable projects;
1. Messaging.Outbox.Producer a console application that produces messages
2. Messaging.OutBox.Consumer1 a console application that consumes messages on the "Outbox-HelloAll" and "Outbox-HelloConsumer1" topics, also runs a secondary background service that writes the current date and time every one seconds, this is to verify that the Kafka consumer isn't blocking the main application
3. Messaging.Outbox.Consumer2 a web API project that consumes messages on the "Outbox-HelloAll" and "Outbox-HelloConsumer2" topics, in addition it provides an endpoint, this is to validate that the consumer isn't blocking the main application and demonstrate how to run a consumer alongside a API

## Things to note
- Snake case naming convention
    - this project uses a Postgres database to hold the outbox data, Postgres uses [Snakecase](https://en.wikipedia.org/wiki/Snake_case) (for example my_table vs pascal case MyTable in MS SQL)
    - you would have to use EntityTypeConfiguraiton .HasColumn or attributes on your models to overcome this
    - a better alternative is to use the https://github.com/efcore/EFCore.NamingConventions library and apply `UseSnakeCaseNamingConvention()`