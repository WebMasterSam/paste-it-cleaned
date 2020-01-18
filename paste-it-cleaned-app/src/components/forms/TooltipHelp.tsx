import React, { Fragment, ReactNode } from 'react'
import HelpOutlineIcon from '@material-ui/icons/HelpOutline'

import './TooltipHelp.less'
import { Tooltip } from '@material-ui/core'
import { TooltipProps } from '@material-ui/core/Tooltip'

export type TooltipHelpProps = TooltipProps

class TooltipHelp extends React.Component<TooltipHelpProps> {
    render() {
        return (
            <div className="tooltip-help">
                <Tooltip {...this.props}>
                    <div className="tooltip-help-inner">
                        {this.props.children}
                        <HelpOutlineIcon className="tooltip-help-icon" />
                    </div>
                </Tooltip>
            </div>
        )
    }
}

export default TooltipHelp
