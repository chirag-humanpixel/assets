// Single File Upload

jQuery(document).ready(function () {
  jQuery("body").on("change", "#single_file", function () {
    $this = jQuery(this);
    file_obj = jQuery($this).prop("files")[0];
    form_data = new FormData();
    form_data.append("file", file_obj);
    form_data.append("action", "single_file_upload");

    console.log(file_obj);
    console.log(form_data);

    jQuery.ajax({
      url: my_ajax_object.ajax_url,
      type: "POST",
      contentType: false,
      processData: false,
      data: form_data,
      success: function (response) {
        alert(response);
      },
    });
  });
});

// Multi File Upload

jQuery(document).ready(function () {
  jQuery("body").on("change", "#multi_file", function () {
    $this = jQuery(this);
    file_obj = $this.prop("files");
    form_data = new FormData();
    for (i = 0; i < file_obj.length; i++) {
      form_data.append("file[]", file_obj[i]);
    }
    form_data.append("action", "multi_file_upload");

    console.log(file_obj);
    console.log(form_data);

    jQuery.ajax({
      url: my_ajax_object.ajax_url,
      type: "POST",
      contentType: false,
      processData: false,
      data: form_data,
      success: function (response) {
        alert(response);
      },
    });
  });
});
