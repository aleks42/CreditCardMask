# CreditCardMask

Includes string formatting and validation.

# Usage

```c#
var helper = new CardNumberHelper()
{
  Mask = "0000 0000 0000 0000",
};

helper.Text = "1234123412341234";
var formattedText = helper.FormattedText; // 1234 1234 1234 1234
var isNumberValid = helper.IsValid; // true
```
# Mask special characters

| char | description                |
| :--- | :--------------------------|
| \*   | optional, any character    |
| ?    | mandatory, any character   |
| 9    | optional, number           |
| 0    | mandatory, number          |
| a    | optional, number or letter |
| A    | mandatory, number or letter|


# Mask examples

| mask                      | example             |
| :------------------------ | :------------------ |
| 0000 0000 0000 0000       | 1234 5678 9012 3456 |
