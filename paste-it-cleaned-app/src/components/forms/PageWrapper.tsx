import React from 'react'

import './PageWrapper.less'

class PageWrapper extends React.Component {
    render() {
        return <div className="page-wrapper">{this.props.children}</div>
    }
}

export default PageWrapper
