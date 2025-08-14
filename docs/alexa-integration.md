# Alexa Integration

This API now includes support for Amazon Alexa skills through the `/alexa` endpoint.

## Features

The Alexa integration provides the following capabilities:

- **Launch Intent**: Welcomes users and explains available functionality
- **Add Numbers Intent**: Performs addition using the existing MathHelpers.Add method
- **Multiply Numbers Intent**: Performs multiplication using the existing MathHelpers.Multiply method
- **Say Hello Intent**: Greets users by name using the existing HelloBuilders.SayHello method
- **Get Weather Intent**: Basic weather response (placeholder)
- **Help Intent**: Provides help information
- **Stop/Cancel Intents**: Handles session termination

## Endpoint

**POST /alexa**

Accepts Alexa skill requests in JSON format and returns appropriate Alexa skill responses.

## Example Usage

The Alexa skill can handle requests like:
- "Alexa, ask My Cool API to add 5 and 3"
- "Alexa, ask My Cool API to multiply 4 and 7"
- "Alexa, ask My Cool API to say hello to Tim"

## Implementation Details

- Uses the `Alexa.NET` package (version 1.22.0) for handling Alexa skill requests and responses
- Integrates with existing helper classes (MathHelpers, HelloBuilders)
- Includes comprehensive error handling
- Provides unit and integration tests

## Testing

The implementation includes both unit tests for the AlexaHelpers class and integration tests for the `/alexa` endpoint.