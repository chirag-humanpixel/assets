const client = new google.auth.JWT(
    key.client_email,
    null,
    key.private_key,
    ['https://www.googleapis.com/auth/spreadsheets']
);
client.authorize(function (err, tokens) {
    if (err) {
        Logger.log.error('Error to get client authorize ' + err.message);
        return;
    } else {
        gsrun(client);
        Logger.log.info('connected to SpreadSheet');
    }
});
async function gsrun(client) {
    const gsapi = google.sheets({ version: 'v4', auth: client });
    await gsapi.spreadsheets.values.append({
        spreadsheetId: spreadsheetId,
        range: 'Sheet1',
        valueInputOption: 'RAW',
        insertDataOption: 'INSERT_ROWS',
        resource: {
            values: [
                [data1,data2] //data = you want to add in RAW
            ],
        },
        auth: client
    });
}