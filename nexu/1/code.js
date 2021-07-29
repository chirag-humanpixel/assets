let persistentMenuAndGetStartedButton = async () => {
    let body = {
        "get_started": {"type":"legacy_reply_to_message_action","message":"Get Started"}
    };
    request({
        "uri": "https://graph.facebook.com/v7.0/me/messenger_profile",
        "qs": { "access_token": access_token},
        "method": "POST",
        "json": body
    }, (err, res, body) => {
        if (!err) {
            Logger.log.info('message sent!');
        } else {
            Logger.log.error("Unable to send message:" + err.message);
        }
    });
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
                        "title": "English",
                        "payload": "CARE_HELP"
                    },
                    {
                        "type": "postback",
                        "title": "Tamil",
                        "payload": "CARE_HELP"
                    }
                ]
            }
        ]
    }
    request({
        "uri": "https://graph.facebook.com/v7.0/me/messenger_profile",
        "qs": { "access_token": access_token },
        "method": "POST",
        "json": request_body
    }, (err, res, body) => {
        if (!err) {
            Logger.log.info('message sent!');
        } else {
            Logger.log.error("Unable to send message:" + err.message);
        }
    });
}