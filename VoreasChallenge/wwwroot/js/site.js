// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {
	var placeholderElement = $('#modal-placeholder');

	$('button[data-toggle="ajax-modal"]').click(function (event) {

		var url = $(this).data('url');
		$.get(url).done(function (data) {	// get処理要求
			placeholderElement.html(data);						// モーダル表示データ設定
			placeholderElement.find('.modal').modal('show');	// モーダル画面表示
		});
	});

	placeholderElement.on('click', '[data-save="modal"]', function (event) {

		event.preventDefault();		// デフォルト処理実施

		var form = $(this).parents('.modal').find('form');
		var actionUrl = form.attr('action');
		var dataToSend = form.serialize();

		$.post(actionUrl, dataToSend).done(function (data) {

			var newBody = $('.modal-body', data);
			placeholderElement.find('.modal-body').replaceWith(newBody);

			var isValid = newBody.find('[name="IsValid"]').val() == 'True';
			if (isValid) {
				placeholderElement.find('.modal').modal('hide');	// モーダル画面非表示
				location.reload();									// 画面再表示
			}
		});
	});
});
