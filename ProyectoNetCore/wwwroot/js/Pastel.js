$(document).ready(function () {
    //peticion
    $.ajax({
        type: "GET",
        contentType: "applicaion/json; charset = utf-8",
        dataType: "json",
        url: "https://localhost:7112/Tienda/DataPastel",
        /*url: urlBase + "/DataPastel", url: urlBase + "/DataPastel",*/
        error: function () {
            alert("Error al cargar los datos")
        },
        success: function (data) {
            console.log(data);
            GraficaPastel(data);
        }
     })
    
})
function GraficaPastel(data) {
    // Build the chart
    Highcharts.chart('pastel', {
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie'
        },
        title: {
            text: 'Browser market shares in March, 2022',
            align: 'left'
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
        },
        accessibility: {
            point: {
                valueSuffix: '%'
            }
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: false
                },
                showInLegend: true
            }
        },
        series: [{
            name: 'Brands',
            colorByPoint: true,
            data: data
        }]
    });
}