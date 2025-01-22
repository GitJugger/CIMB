$(document).ready(function () {

    formatDate();
    displayDate();
    example();
});
function example() {

    var buttonsExcel = [
        {
            extend: 'excelHtml5',
            filename: "AccessMatrix_" + formatDate(),
            title: "Role Access Matrix Report",
            "createEmptyCells": true,
            exportOptions: {
            // Customize export of checkbox column
            format: {
                body: function (data, row, column, node) {
                    // For checkbox column
                    if ($(node).find('input[type="checkbox"]').length) {
                        return $(node).find('input[type="checkbox"]').is(':checked') ? 'Y' : '';
                    }
                    return data;
                }
            }
        },

            customize: function (xlsx) {
                var sheet = xlsx.xl.worksheets['sheet1.xml'];

                $('row c', sheet).attr('s', '25');

                var styles = xlsx.xl['styles.xml'];
                var fonts = $('fonts', styles);
                var newFontId = parseInt(fonts.attr('count'));
                fonts.attr('count', newFontId + 1).append('<font><sz val="22"></sz><name val="Calibri"></name><b></b></font>');
                var cellXfs = $('cellXfs', styles);
                var newStyleId = parseInt(cellXfs.attr('count'));
                cellXfs.attr('count', newStyleId + 1).append('<xf numFmtId="0" fontId="' + newFontId + '" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyAlignment="1"><alignment horizontal="center"></alignment></xf>');

                var rowCount = 0;
                $('row', sheet).each(function () {
                    var row = this;
                    if (rowCount == 0) {
                        $('c[r="A1"]', row).attr('s', newStyleId);
                    }
                    rowCount++;
                });

                $('row c[r="A6"]', sheet).attr('s', '37');
                $('row c[r="B6"]', sheet).attr('s', '37');
                $('row c[r="C6"]', sheet).attr('s', '37');
                $('row c[r="D6"]', sheet).attr('s', '37');
                $('row c[r="E6"]', sheet).attr('s', '37');
                $('row c[r="F6"]', sheet).attr('s', '37');
                $('row c[r="G6"]', sheet).attr('s', '37');
                $('row c[r="H6"]', sheet).attr('s', '37');
                $('row c[r="I6"]', sheet).attr('s', '37');
                $('row c[r="J6"]', sheet).attr('s', '37');
                $('row c[r="K6"]', sheet).attr('s', '37');
                $('row c[r="L6"]', sheet).attr('s', '37');
                $('row c[r="M6"]', sheet).attr('s', '37');
                $('row c[r="N6"]', sheet).attr('s', '37');
                $('row c[r="O6"]', sheet).attr('s', '37');


                $('row c[r="A2"]', sheet).attr('s', '0');
                $('row c[r="A3"]', sheet).attr('s', '0');
                $('row c[r="A4"]', sheet).attr('s', '0');
                $('row c[r="A4"]', sheet).attr('s', '0');
                $('row c[r="A5"]', sheet).attr('s', '0');

                $('row c[r="B2"]', sheet).attr('s', '0');
                $('row c[r="B3"]', sheet).attr('s', '0');
                $('row c[r="B4"]', sheet).attr('s', '0');
                $('row c[r="B4"]', sheet).attr('s', '0');
                $('row c[r="B5"]', sheet).attr('s', '0');


                $('row c[r="C2"]', sheet).attr('s', '0');
                $('row c[r="C3"]', sheet).attr('s', '0');
                $('row c[r="C4"]', sheet).attr('s', '0');
                $('row c[r="C4"]', sheet).attr('s', '0');
                $('row c[r="C5"]', sheet).attr('s', '0');

                $('row c[r="D2"]', sheet).attr('s', '0');
                $('row c[r="D3"]', sheet).attr('s', '0');
                $('row c[r="D4"]', sheet).attr('s', '0');
                $('row c[r="D4"]', sheet).attr('s', '0');
                $('row c[r="D5"]', sheet).attr('s', '0');


                $('row c[r="E2"]', sheet).attr('s', '0');
                $('row c[r="E3"]', sheet).attr('s', '0');
                $('row c[r="E4"]', sheet).attr('s', '0');
                $('row c[r="E4"]', sheet).attr('s', '0');
                $('row c[r="E5"]', sheet).attr('s', '0');

                $('row c[r="F2"]', sheet).attr('s', '0');
                $('row c[r="F3"]', sheet).attr('s', '0');
                $('row c[r="F4"]', sheet).attr('s', '0');
                $('row c[r="F4"]', sheet).attr('s', '0');
                $('row c[r="F5"]', sheet).attr('s', '0');

                $('row c[r="G2"]', sheet).attr('s', '0');
                $('row c[r="G3"]', sheet).attr('s', '0');
                $('row c[r="G4"]', sheet).attr('s', '0');
                $('row c[r="G4"]', sheet).attr('s', '0');
                $('row c[r="G5"]', sheet).attr('s', '0');

                $('row c[r="H2"]', sheet).attr('s', '0');
                $('row c[r="H3"]', sheet).attr('s', '0');
                $('row c[r="H4"]', sheet).attr('s', '0');
                $('row c[r="H4"]', sheet).attr('s', '0');
                $('row c[r="H5"]', sheet).attr('s', '0');

                $('row c[r="I2"]', sheet).attr('s', '0');
                $('row c[r="I3"]', sheet).attr('s', '0');
                $('row c[r="I4"]', sheet).attr('s', '0');
                $('row c[r="I4"]', sheet).attr('s', '0');
                $('row c[r="I5"]', sheet).attr('s', '0');

                $('row c[r="J2"]', sheet).attr('s', '0');
                $('row c[r="J3"]', sheet).attr('s', '0');
                $('row c[r="J4"]', sheet).attr('s', '0');
                $('row c[r="J4"]', sheet).attr('s', '0');
                $('row c[r="J5"]', sheet).attr('s', '0');

                $('row c[r="K2"]', sheet).attr('s', '0');
                $('row c[r="K3"]', sheet).attr('s', '0');
                $('row c[r="K4"]', sheet).attr('s', '0');
                $('row c[r="K4"]', sheet).attr('s', '0');
                $('row c[r="K5"]', sheet).attr('s', '0');

                $('row c[r="L2"]', sheet).attr('s', '0');
                $('row c[r="L3"]', sheet).attr('s', '0');
                $('row c[r="L4"]', sheet).attr('s', '0');
                $('row c[r="L4"]', sheet).attr('s', '0');
                $('row c[r="L5"]', sheet).attr('s', '0');

                $('row c[r="M2"]', sheet).attr('s', '0');
                $('row c[r="M3"]', sheet).attr('s', '0');
                $('row c[r="M4"]', sheet).attr('s', '0');
                $('row c[r="M4"]', sheet).attr('s', '0');
                $('row c[r="M5"]', sheet).attr('s', '0');

                $('row c[r="N2"]', sheet).attr('s', '0');
                $('row c[r="N3"]', sheet).attr('s', '0');
                $('row c[r="N4"]', sheet).attr('s', '0');
                $('row c[r="N4"]', sheet).attr('s', '0');
                $('row c[r="N5"]', sheet).attr('s', '0');

                $('row c[r="O2"]', sheet).attr('s', '0');
                $('row c[r="O3"]', sheet).attr('s', '0');
                $('row c[r="O4"]', sheet).attr('s', '0');
                $('row c[r="O4"]', sheet).attr('s', '0');
                $('row c[r="O5"]', sheet).attr('s', '0');

               

                var col = $("col", sheet);
                $(col[0]).attr("width", 20);

                var mergeCells = $('mergeCells', sheet);

                mergeCells[0].appendChild(_createNode(sheet, 'mergeCell', {
                    attr: {
                        ref: 'A3:B3'
                    }
                }));

                mergeCells[0].appendChild(_createNode(sheet, 'mergeCell', {
                    attr: {
                        ref: 'A4:B4'
                    }
                }));

                mergeCells.attr('count', mergeCells.attr('count') + 1);

                function _createNode(doc, nodeName, opts) {
                    var tempNode = doc.createElement(nodeName);

                    if (opts) {
                        if (opts.attr) {
                            $(tempNode).attr(opts.attr);
                        }

                        if (opts.children) {
                            $.each(opts.children, function (key, value) {
                                tempNode.appendChild(value);
                            });
                        }

                        if (opts.text !== null && opts.text !== undefined) {
                            tempNode.appendChild(doc.createTextNode(opts.text));
                        }
                    }

                    return tempNode;
                }

            }
        }

    ];

    var buttonsPdf = [
        {
            extend: 'pdfHtml5',
            title: " ",
            filename: "AccessMatrix_" + formatDate(),
            orientation: 'landscape', //portrait
            pageSize: 'A4', //A3 , A5 , A6 , legal , letter
            exportOptions: {
                columns: ':visible',
                search: 'applied',
                order: 'applied',
                format: {
                    body: function (data, row, column, node) {
                        // For checkbox column
                        if ($(node).find('input[type="checkbox"]').length) {
                            return $(node).find('input[type="checkbox"]').is(':checked') ? 'Y' : '';
                        }
                        return data;
                    }
                }
            },
            customize: function (doc) {

                doc.pageMargins = [20, 60, 20, 30];
                doc.defaultStyle.fontSize = 8;
                doc.styles.tableHeader.fontSize = 10;

                var now = new Date();
                var jsDate = now.getDate() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear();

                doc['header'] = (function () {
                    return {
                        columns: [
                            {
                                alignment: 'center',
                                fontSize: 14,
                                text: 'Role Access Matrix Report',
                                bold: true
                            }
                        ],
                        margin: 20
                    }
                });


                doc['footer'] = (function (page, pages) {
                    return {
                        columns: [
                            {
                                alignment: 'right',
                                text: ['page ', { text: page.toString() }, ' of ', { text: pages.toString() }]
                            }
                        ],
                        margin: 20
                    }
                });


                doc.content[1].table.body[1][0].alignment = 'left';
                doc.content[1].table.body[2][0].alignment = 'left';



                doc.content[1].table.body[4][0].fillColor = '#FF8674';
                doc.content[1].table.body[4][1].fillColor = '#FF8674';
                doc.content[1].table.body[4][2].fillColor = '#FF8674';
                doc.content[1].table.body[4][3].fillColor = '#FF8674';
                doc.content[1].table.body[4][4].fillColor = '#FF8674';
                doc.content[1].table.body[4][5].fillColor = '#FF8674';
                doc.content[1].table.body[4][6].fillColor = '#FF8674';
                doc.content[1].table.body[4][7].fillColor = '#FF8674';
                doc.content[1].table.body[4][8].fillColor = '#FF8674';
                doc.content[1].table.body[4][9].fillColor = '#FF8674';
                doc.content[1].table.body[4][10].fillColor = '#FF8674';
                doc.content[1].table.body[4][11].fillColor = '#FF8674';
                doc.content[1].table.body[4][12].fillColor = '#FF8674';
                doc.content[1].table.body[4][13].fillColor = '#FF8674';
                doc.content[1].table.body[4][14].fillColor = '#FF8674';

                doc.content[1].layout = "borders";

                doc.content[1].table.body[0][0].border = [false, false, false, false];
                doc.content[1].table.body[1][0].border = [false, false, false, false];
                doc.content[1].table.body[2][0].border = [false, false, false, false];
                doc.content[1].table.body[3][0].border = [false, false, false, false];

                doc.content[1].table.body[1][0].hLineWidth = "20%";

                var objLayout = {};
                objLayout['hLineWidth'] = function (i) { return .5; };
                objLayout['vLineWidth'] = function (i) { return .5; };
                objLayout['hLineColor'] = function (i) { return '#aaa'; };
                objLayout['vLineColor'] = function (i) { return '#aaa'; };
                objLayout['paddingLeft'] = function (i) { return 4; };
                objLayout['paddingRight'] = function (i) { return 4; };

                doc.content[0].layout = objLayout;
            }
        }

    ];

    var buttonsNew = [
        {
            extend: 'collection',
            text: 'Export',
            autoClose: true,
            searching: false,
            info: false,
            ordering: false,

            buttons: [buttonsExcel[0], buttonsPdf[0]]

        }
    ];

    var table = new DataTable('#example', {

        pageLength: 10,
        searching: true,
        paging: true,
        info: true,
        ordering: false,
        responsive: true,

        layout: {
            topStart: {
                buttons: buttonsNew
            }
        }
    });
}


function formatDate() {
    let date = new Date();

    let day = String(date.getDate()).padStart(2, '0');
    let month = String(date.getMonth() + 1).padStart(2, '0'); // Months are 0-based in JavaScript
    let year = date.getFullYear();

    let hours = String(date.getHours()).padStart(2, '0');
    let minutes = String(date.getMinutes()).padStart(2, '0');
    let seconds = String(date.getSeconds()).padStart(2, '0');

    return `${day}/${month}/${year} ${hours}:${minutes}:${seconds}`;
}

function displayDate() {
    document.getElementById("dateTime").innerHTML = formatDate();
}
