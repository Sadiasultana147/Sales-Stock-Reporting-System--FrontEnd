function RequiredSubmitFunction() {
    var isValid = true;
    $('input[type="text"],select,textarea').each(function () {   // Loop thru all the elements    
        if (this.required) {
            var name = $(this).val();
            if (this.type.match(/select.*/) && name == "0") {
                name = "";
            }
            if (name == "") {  // If not empty do nothing
                RequiredFieldValidate(this.id);
                isValid = false;
                return isValid;
            } else {
                isValid = true;
                return isValid;
            }
        }
    });
    return isValid;
}
