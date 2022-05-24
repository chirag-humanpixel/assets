let request_body = {
    "persistent_menu": [
        {
            "locale": "default",
            "composer_input_disabled": false,
            "call_to_actions": [
                {
                    "type": "postback",
                    "title": "Restart Bot",
                    "payload": "CARE_HELP"
                },
                {
                    "type": "postback",
                    "title": "Select Language",
                    "payload": "CARE_HELP"
                }
            ]
        }
    ]
}
try {
    let menu = await axios({
        method: "post",
        url: `https://graph.facebook.com/v7.0/me/messenger_profile?access_token=${config.facebook.fbPageAccessToken}`,
        headers: {
            'Accept': '*/*',
            'Content-Type': 'application/json'
        },
        data: request_body
    });
} catch (err) {
    Logger.log.error('Error in persistentMenu' + err.response.data);
}