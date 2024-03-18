$(document).ready(function () {
    //peticion
    $.ajax({
        type: "GET",
        contentType: "applicaion/json; charset = utf-8",
        dataType: "json",
        /*url: "https://localhost:7112/Tienda/DataPastel",*/
        url: urlBase,
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
            text: 'Porcentaje de acciones compradas por empresa',
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
            name: 'Porcentaje',
            colorByPoint: true,
            data: data
        }]
    });
}