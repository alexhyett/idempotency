# Idempotency - What is it and How to Implement It
An example API showing Idempotency using Redis.

This is the code that is shown in my video "Idempotency - What is it and How to Implement It".

To run this project you need to do the following:

1. Run `docker-compose up` to start the Redis container.
2. Run `dotnet run` to start the API.

You can then send requests like the following:

```sh
curl --location 'http://localhost:5000/payment/' \
--header 'IdempotencyKey: A222EB7F-E861-420F-B010-0EDDD29C935E' \
--header 'Content-Type: application/json' \
--data '{
  "amount": 1200,
  "currency": "USD",
  "cardNumber": "4122916040150031",
  "cvv": "123",
  "expiryYear": "2023",
  "expiryMonth": "01"
}'
```

If you send this request through multiple times you will get the same response.

Such as:

```json
{
    "id": "e5c68b39-b534-47c1-a672-a6f8c1fd18f6",
    "amount": 1200,
    "currency": "USD",
    "cardNumber": "************0031",
    "created": "2023-09-20T11:08:34.028706Z"
}
```

You can also retrieve payments using GET and the ID:
```sh
curl --location 'http://localhost:5000/payment/e5c68b39-b534-47c1-a672-a6f8c1fd18f6'
```

