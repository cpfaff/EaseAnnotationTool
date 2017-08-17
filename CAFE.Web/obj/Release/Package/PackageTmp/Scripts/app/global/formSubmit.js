
//leave the submit button clickable if the client side validation fails
$(document).on("invalid-form.validate", "form", function () {
	var button = $(this).find('input[type="submit"]');
	setTimeout(function () {
		button.removeAttr("disabled");
	}, 1);
});
//disable the submit button after click
$(document).on("submit", "form", function () {
	var button = $(this).find('input[type="submit"]');
	setTimeout(function () {
		button.attr("disabled", "disabled");
	}, 0);
});
