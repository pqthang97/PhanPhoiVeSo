var common = {
	init: function () {
		common.registerEvent();
	},
	registerEvent: function () {
		$("#miniKeyword").autocomplete({
			source: function (request, response) {
				$.ajax({
					url: "/Product/ListName",
					dataType: "json",
					minLength: 2,
					data: {
						q: request.term
					},
					success: function (res) {
						response(res.data);
					}
				});
			},
			focus: function (event, ui) {
				$("#miniKeyword").val(ui.item.label);
				return false;
			},
			select: function (event, ui) {
				$("#miniKeyword").val(ui.item.label);
				return false;
			}
		})
			.autocomplete("instance").data("ui-autocomplete")._renderItem = function (ul, item) {
				return $("<li>")
					.append("<a>" + item.label + "</a>")
					.appendTo(ul);
			};
	}
}
common.init();