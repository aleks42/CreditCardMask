# CreditCardMask

Includes string formatting and validation.

# Usage

```c#
var helper = new CardNumberHelper();
helper.Mask = "0000 0000 0000 0000";
helper.Text = "1234123412341234";

var formattedText = helper.FormattedText; // 1234 1234 1234 1234
var isNumberValid = helper.IsValid; // true
```
# Mask special characters

| char | description              |
| :--- | :----------------------- |
| \*   | mandatory, any character |
| ?    | mandatory, letter        |
| 0    | mandatory, number        |
| 9    | optional, number         |

# Mask examples

| mask                      | example             |
| :------------------------ | :------------------ |
| 0000 0000 0000 0000       | 1234 5678 9012 3456 |
| 0000 0000 0000 0000 999   |                     |
| ****************          | efbbbf0d0a4d6963    |
| ??-??                     | ru-ru               |
| 00/00/0000 00:00          | 01/10/2008 17:04    |
| 0000-00-00 00:00:00       | 2008-10-01 17:04:32 |
