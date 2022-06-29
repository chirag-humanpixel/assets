import PropTypes from "prop-types"
import CustomSelect from "../../../common/CustomSelect/CustomSelect"
import Input from "../../../common/Input/Input"

const RepeaterFields = ({ field }) => {
  const { type, options, question, inputType, fieldName } = field
  return (
    <>
      {(() => {
        switch (type) {
          case "text":
            return (
              <Input
                type={inputType}
                className=""
                name={fieldName}
                // value={}
                placeholder={question}
                // onChange={e => console.log(e)}
              />
            )
          case "dropdown": {
            const optionData = options?.map(e => ({ label: e, value: e })) ?? []
            const value = optionData?.find(e => e?.value === "")
            return (
              <CustomSelect
                options={optionData}
                name={fieldName}
                className="repeater-select"
                value={value}
                placeholder={question}
                onChangeCustomSelect={e => {}}
              />
            )
          }
          default:
            return null
        }
      })()}
    </>
  )
}

RepeaterFields.propTypes = {
  field: PropTypes.object.isRequired,
}

export default RepeaterFields
