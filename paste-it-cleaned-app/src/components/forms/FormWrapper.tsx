import React from "react";

import "./FormWrapper.less";

class FormWrapper extends React.Component {
  render() {
    return <div className="form-wrapper">{this.props.children}</div>;
  }
}

export default FormWrapper;
