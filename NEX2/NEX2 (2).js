/**
 * 
 * @param {*} client client data
 * @param {*} access_token access token
 */
async function gsrun(client, access_token) {
    //collect organization data
    const organizationData = await Organization.findById({ _id: organizationId }).select('spreadSheetId');
    const gsapi = google.sheets({ version: 'v4', auth: client });
    try {
        let userLanguage = '';
        if (userContext.data.parameters.language === 'en-US') {
            userLanguage = 'English';
        } else if (userContext.data.parameters.language === 'fr') {
            userLanguage = 'Dari';
        } else {
            userLanguage = 'Tamil';
        }
        gsapi.spreadsheets.values.append({
            spreadsheetId: organizationData.spreadSheetId,
            range: 'Output RAW',
            valueInputOption: 'RAW',
            insertDataOption: 'INSERT_ROWS',
            resource: {
                values: [
                    [
                        userContext.data.parameters.userName,
                        userContext.data.parameters.phoneNumber,
                        userLanguage,
                        userContext.data.parameters.country,
                        userContext.data.parameters.nationality,
                        userContext.data.parameters.currentLocation,
                        userContext.data.parameters.age,
                        userContext.data.parameters.migrating,
                        userContext.data.parameters.visa,
                        userContext.data.parameters.countryInsideOrOutside,
                        userContext.data.parameters.airOrBoat,
                        userContext.data.parameters.visaStatus,
                        time,
                    ],
                ],
            },
            auth: client,
        });
    } catch (err) {
        Logger.log.error('Error insert Raw in Google Spreadsheet ' + err.message);
        Logger.log.error(err.response.data);
    }
}