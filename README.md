# CreditCardMask

Includes number formatting and validation.

# Usage

```c#
var cardNumberHelper = new CardNumberHelper();
cardNumberHelper.Mask = "0000 0000 0000 0000";
cardNumberHelper.Text = "1234123412341234";

var formattedText = cardNumberHelper.FormattedText;
var isNumberValid = cardNumberHelper.IsValid;
```