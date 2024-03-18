$(document).ready(function () {
    //peticion
    $.ajax({
        type: "GET",
        contentType: "applicaion/json; charset = utf-8",
        dataType: "json",
        /*url: "https://localhost:7112/Tienda/DataBarras",*/
        url: urlBase,
        error: function () {
            alert("Error al cargar los datos")
        },
        success: function (data) {
            console.log(data);
            GraficaBarras(data);
        }
    })

})

function GraficaBarras(data) {

    Highcharts.chart('barras', {
        chart: {
            type: 'column'
        },
        title: {
            text: 'Compras realizadas en 2023'
        },
        subtitle: {
            text: ''
        },
        xAxis: {
            type: 'category',
            labels: {
                autoRotation: [-45, -90],
                style: {
                    fontSize: '13px',
                    fontFamily: 'Verdana, sans-serif'
                }
            }
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Population (millions)'
            }
        },
        legend: {
            enabled: false
        },
        tooltip: {
            pointFormat: 'Compra: <b>{point.y:.1f} k</b>'
        },
        series: [{
            name: 'Population',
            colors: [
                '#9b20d9', '#9215ac', '#861ec9', '#7a17e6', '#7010f9', '#691af3',
                '#6225ed', '#5b30e7', '#533be1', '#4c46db', '#4551d5', '#3e5ccf',
                '#3667c9', '#2f72c3', '#277dbd', '#1f88b7', '#1693b1', '#0a9eaa',
                '#03c69b', '#00f194'
            ],
            colorByPoint: true,
            groupPadding: 0,
            data: data ,
            dataLabels: {
                enabled: true,
                rotation: -90,
                color: '#FFFFFF',
                inside: true,
                verticalAlign: 'top',
                format: '{point.y:.1f}', // one decimal
                y: 10, // 10 pixels down from the top
                style: {
                    fontSize: '13px',
                    fontFamily: 'Verdana, sans-serif'
                }
            }
        }]
    });

}