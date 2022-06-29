import PropTypes from "prop-types"
import { RenderField } from "./RenderField"

const RepeaterForm = ({ field, onFieldUpdate, errorMessage, onAdd, onRemove }) => {
  const { question, fields, fieldName } = field

  const handleFieldUpdate = (key, value, path, index) => {
    onFieldUpdate(fieldName, value, path, true, index, key)
  }

  const canAdd = () => {
    if (fieldName === "income") {
      const answers = fields?.map(e => e?.find(field => field?.fieldName === "incomeType")?.answer)
      if (answers?.includes("Spousal Wages After Tax")) return false
    }
    return true
  }

  const handleAdd = () => {
    if (canAdd()) {
      const updatedFields = [...fields]
      updatedFields.push(
        fields?.[0]?.map(field => ({
          ...field,
          answer: undefined,
        })),
      )
      onAdd(updatedFields)
    }
  }

  const handleRemove = key => {
    const updatedFields = [...fields].filter((_, index) => key !== index)
    onRemove(updatedFields)
  }

  return (
    <div className="repeater-wrapper repeater-questions">
      <div className="repeater-head">
        {question}
        <span className="material-icons-outlined repeater-add-icon" onClick={handleAdd}>
          control_point
        </span>
      </div>
      <table className="repeater-table">
        <tr>
          {fields?.length > 1 && <th></th>}
          {fields?.[0].map(e => (
            <th key={e.fieldName}>{e.question}</th>
          ))}
        </tr>
        {fields?.map((f, index) => (
          <tr key={index}>
            {fields?.length > 1 && (
              <td>
                <span className="material-icons-outlined repeater-remove-icon" onClick={() => handleRemove(index)}>
                  remove_circle_outline
                </span>
              </td>
            )}
            {f.map(e => (
              <td key={e.fieldName} style={{ minWidth: "200px" }}>
                <RenderField
                  field={e}
                  indexForRepeater={index}
                  onFieldUpdate={(key, value, path) => handleFieldUpdate(key, value, path, index)}
                  errorMessage={errorMessage?.[index]?.[e?.fieldName] || ""}
                />
              </td>
            ))}
          </tr>
        ))}
      </table>
    </div>
  )
}

RepeaterForm.propTypes = {
  // field to show
  field: PropTypes.object,
  // call when field changes
  onFieldUpdate: PropTypes.func,
  errorMessage: PropTypes.string,
  handleSearch: PropTypes.func,
  onAdd: PropTypes.func,
  onRemove: PropTypes.func,
}

export default RepeaterForm
