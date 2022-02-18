# Tribal Practical Exercise - CreditLineAnalyserApp

## Summary

The `CreditLineAnalyserApp` is a very simple API that analyses the credit options provided to its endpoint.

## How to build and run the application

With docker and docker-compose installed on your system, run the following command from the root folder of the project:

`docker-compose up -d --build`


## Using the API

To test the application you access the swagger enpoint at `localhost:5086/swagger/index.html` or use an http client such as `Postman` or `Insomnia`.

To check if requested credit line is accepted, send a `post` request to the endpoint: `localhost:5086/api/CreditLine` with the payload with the format:
```json
{
  "foundingType": "SME",
  "cashBalance": 435.30,
  "monthlyRevenue": 4235.45,
  "requestedCreditLine": 100,
  "requestedDate": "2021-07-19T16:32:59.860Z"
}
```
