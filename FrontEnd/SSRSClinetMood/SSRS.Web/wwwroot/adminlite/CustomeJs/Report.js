$(document).ready(function () {
    $('#btnCurrentStock').click(function (e) {
        e.preventDefault();
        window.open('/ReportingModule/Report/PdfGenerates', '_blank');
    });
    // Date-wise Stock button
    $('#btnDateWiseStock').click(function (e) {
        e.preventDefault();

        var fromDate = $('#FromDate').val();
        var toDate = $('#ToDate').val();

        if (!fromDate || !toDate) {
            alert("Please select both From Date and To Date.");
            return;
        }

        // Open report in new tab with dates as query string
        var reportUrl = `/ReportingModule/Report/dateWisePdfGenerates?fromDate=${fromDate}&toDate=${toDate}`;
        window.open(reportUrl, '_blank');
    });
});