
google.charts.load('current', {'packages':['corechart']});
google.charts.setOnLoadCallback(drawChart);

function drawChart() {
	var data = new google.visualization.DataTable();
	data.addColumn('number', '年齢(歳)');
	data.addColumn('number', '身長(cm)');
	data.addColumn('number', '体重(kg)');

	data.addRows([
		[1, 76, 10],
		[2, 85, 12],
		[3, 94, 14],
		[4, 100, 16],
		[5, 105, 18],
		[6, 112, 20],
		[7, 120, 22],
		[8, 125, 25],
		[9, 130, 29],
		[10, 135, 32],
		[11, 140, 38],
		[12, 149, 41],
		[13, 156, 48],
		[14, 161, 52],
		[15, 168, 58],
		[16, 169, 60],
		[17, 171, 62]
	]);

	var options = {
		title: '標準身長・体重曲線 男子(SD表示)',
		curveType: 'function',
		legend: { position: 'top' },
		series: [
			{ type: 'line', targetAxisIndex: 0 },
			{ type: 'line', targetAxisIndex: 1 }
		],
		vAxes: [
			{ title: '身長(cm)' },
			{ title: '体重(kg)' }
		],
		hAxis: {
			title: '年齢(歳)',
			minValue: 0,
			maxValue: 23,
			gridlines: { count: 11 }
		},
		vAxis: {
			minValue: 0,
			maxValue: 200,
			gridlines: { count: 20 }
		}
	};

	var chart = new google.visualization.LineChart(document.getElementById('taikaku_chart'));
	chart.draw(data, options);
}
 