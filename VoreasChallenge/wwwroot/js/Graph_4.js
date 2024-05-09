
google.charts.load('current', {'packages':['corechart']});
google.charts.setOnLoadCallback(drawChart);

function drawChart() {
	var data = new google.visualization.DataTable();
	data.addColumn('string', '年齢(歳)');
	data.addColumn('number', '1.5SD');
	data.addColumn('number', '0.5SD');
	data.addColumn('number', '-0.5SD');
	data.addColumn('number', '-1.5SD');

	data.addRows([
		['3', 5.0, 5.2, 5.5, 5.8],
		['4', 4.8, 5.0, 5.2, 5.5],
		['5', 4.6, 4.8, 5.0, 5.3],
		['6', 4.4, 4.6, 4.8, 5.0],
		['7', 4.2, 4.4, 4.6, 4.8],
		['8', 4.0, 4.2, 4.4, 4.6],
		['9', 4.0, 4.1, 4.2, 4.4],
		['10', 3.9, 4.0, 4.1, 4.2],
		['11', 3.8, 3.9, 4.0, 4.1],
		['12', 3.7, 3.8, 3.9, 4.0],
		['13', 3.6, 3.7, 3.8, 3.9],
		['14', 3.4, 3.5, 3.6, 3.7],
		['15', 3.2, 3.3, 3.4, 3.5],
		['16', 3.2, 3.3, 3.3, 3.4],
		['17', 3.1, 3.0, 3.2, 3.3],
		['18', 3.0, 3.1, 3.1, 3.2],
		['19', 3.0, 3.1, 3.1, 3.1],
		['20', 3.0, 3.1, 3.1, 3.1],
		['21', 2.9, 3.0, 3.0, 3.1],
		['22', 2.9, 3.0, 3.0, 3.1]
	]);

	// Set chart options
	var options = {
		'title':'',
		curveType: 'function',
		legend: { position: 'top' }
	};

	var chart = new google.visualization.ComboChart(document.getElementById('noryoku4_chart'));
	chart.draw(data, options);
}
 