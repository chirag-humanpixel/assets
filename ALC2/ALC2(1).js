/**
 * Models
 * */
const userBotUser = mongoose.model('user-botuser');
/**
 * 
 * @param {*} userText bot User text
 * @param {*} intentName Intent name
 * @param {*} userId Bot user Id
 * @param {*} sessionId Bot user Session Id
 * @param {*} organizationId User Organization Id
 */
let conversitionDataUpdate = async (userText, intentName, userId, sessionId, organizationId) => {
    console.log(userText, intentName, userId);
    if (intentName == 'gather_request_user_email' ||
        intentName == 'gather_request_user_email_send_us_a_message') {
        await userBotUser.update({ userId: userId }, { 'conversitionData.drugFactsQuery': userText });
    }
};