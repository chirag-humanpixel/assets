// $(document).ready(function () {
//   $(".name_edit_repeater").repeater({
//     initEmpty: true,

//     defaultValues: {
//       "text-input": "foo",
//     },

//     show: function () {
//       $(this).slideDown();
//     },

//     hide: function (deleteElement) {
//       if (confirm("Are you sure you want to delete this element?")) {
//         $(this).slideUp(deleteElement);
//       }
//     },

//     ready: function (setIndexes) {
//       $dragAndDrop.on("drop", setIndexes);
//     },

//     isFirstItemUndeletable: true,
//   });
// });

jQuery(document).ready(function () {
  jQuery(".repeater").repeater({
    initEmpty: false,
    show: function () {
      jQuery(this).slideDown();
    },
    hide: function (deleteElement) {
      jQuery(this).slideUp(deleteElement);
    },
    ready: function (setIndexes) {},
    isFirstItemUndeletable: false,
  });
});
