const { google } = require('googleapis');
const key = require('ga.json'); //google analytics json file
let jwtClient = new google.auth.JWT(
    key.client_email, // For authenticating and permissions
    null,
    key.private_key,
    ['https://www.googleapis.com/auth/analytics.readonly'],
    null,
);

jwtClient.authorize(function (err, tokens) {
    if (err) {
        Logger.log.error('jwtClient is not authorized.');
        Logger.log.error(err);
        return;
    } else {
        Logger.log.info('jwtClient is authorized.');
    }
});
dimensions_rows = [
    {
        name: 'ga:userAgeBracket',
    },
];

date_filters = [
    {
        startDate: startDateForGoogleAnalytics, //'2021-05-01',
        endDate: endDateForGoogleAnalytics, //'2021-06-24',
    },
];

var req = {
    reportRequests: [
        {
            viewId: googleAnalytics.viewId, // Google analytics ViewId
            dateRanges: date_filters,
            dimensions: dimensions_rows,
        },
    ],
};
// Pull report and output the data
analytics.reports.batchGet(
    {
        auth: jwtClient,
        resource: req,
    },
    function (err, response) {
        if (err) {
            Logger.log.error(err);
            Logger.log.error('Failed to get Report.');
            return;
        }
        console.log(response.data);
    },
);