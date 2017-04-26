# YoungGuns

## YoungGuns.UI
Two steps: first step is that the user creates their form, setting up all their fields. The form will be comprised of info fields and calc fields. When they are done creating the form, we will POST the entire form to the api with the following request body:

```js
{
    "form_id": 1,
    "tax_system": "My Form",
    "form_fields": [
        {
          "field_title": "Info Field",
          "field_type": "textfield",
          "field_value": "500",
          "field_calculation": null,
          "type": "info"
        },
        {
          "field_title": "Calc Field",
          "field_type": "calcfield",
          "field_value": null,
          "field_calculation": "[Info Field] * 2",
          "type": "calc"
        }
    ]
}
```
