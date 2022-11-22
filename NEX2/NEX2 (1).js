/**
 * 
 * @param {*} sessionId user Session Id
 * @param {*} organizationId organization Id
 * @returns {object}
 */
let languageIdentification = async (sessionId, organizationId) => {
    let message;
    let time = new Date();
    Logger.log.info('user Messege time ' + time);

    const df = await dialogflowHelper.getGoogleToken(organizationId); //Get organization Dialogflow data
    let isContextFromGetAPI = true;
    try {
        userContext = await dialogflowHelper.getSpecificContext(
            df.projectId,
            sessionId,
            'customContext',
            df.accessToken,
        );
    } catch (e) {
        Logger.log.info('In create custom context');
        try {
            // message = 'hi';
            isContextFromGetAPI = false;
            Logger.log.info('user Info get Successfully');
            let createContext = {
                name: `projects/${df.projectId}/agent/sessions/${sessionId}/contexts/customContext`,
                lifespanCount: 15,
                parameters: {
                    userLastMessege: '',
                    userFallback: true,
                    userName: '',
                    language: 'en-US',
                    nationality: '',
                    currentLocation: '',
                    age: '',
                    migrating: '',
                    time: time,
                    country: '',
                    visa: '',
                    countryInsideOrOutside: '',
                    airOrBoat: '',
                    visaStatus: '',
                    phoneNumber: '',
                },
            };
            userContext = await dialogflowHelper.setContext(createContext, sessionId, df.accessToken, df.projectId);
        } catch (err) {
            Logger.log.error('Error in Create custom context ' + err.message);
            Logger.log.error(err.response.data);
        }
    }

    return {
        languageCode:
            userContext.data && userContext.data.parameters && userContext.data.parameters.language
                ? userContext.data.parameters.language
                : 'en-US',
        message,
        userContext,
        isContextFromGetAPI,
    };
};