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

## Things to note
- Snake case naming convention
    - this project uses a Postgres database to hold the outbox data, Postgres uses [Snakecase](https://en.wikipedia.org/wiki/Snake_case) (for example my-table vs pascal case MyTable in MS SQL)
    - you would have to use EntityTypeConfiguraiton .HasColumn or attributes on your models to overcome this
    - a better alternative is to use the https://github.com/efcore/EFCore.NamingConventions library and apply `UseSnakeCaseNamingConvention()`