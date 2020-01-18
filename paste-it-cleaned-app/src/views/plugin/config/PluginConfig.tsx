import React from 'react'

import { FormControlLabel, Switch, Grid, Paper, Typography } from '@material-ui/core'
import SettingsIcon from '@material-ui/icons/Settings'
import ErrorOutlineIcon from '@material-ui/icons/ErrorOutline'

import TooltipHelp from '../../../components/forms/TooltipHelp'
import { ConfigEntity } from '../../../entities/api'
import { PluginConfigController } from './PluginConfigController'

import './PluginConfig.less'
import { Skeleton } from '@material-ui/lab'
import LoadingError from '../../../components/forms/LoadingError'

export interface PluginConfigProps {}
export interface PluginConfigState {
    isLoaded: boolean
    config: ConfigEntity
    configLoading: boolean
    configError: boolean
}

class PluginConfig extends React.Component<PluginConfigProps, PluginConfigState> {
    private controller?: PluginConfigController = undefined

    constructor(props: PluginConfigProps) {
        super(props)
        this.controller = new PluginConfigController(this)
        this.state = {
            isLoaded: false,
            config: {} as ConfigEntity,
            configLoading: false,
            configError: false,
        }
    }

    componentDidMount() {
        this.controller!.initialize()
    }

    render() {
        return (
            <React.Fragment>
                <Paper className="paper">
                    <Typography variant="h2" className="override-h2" component="h2">
                        <SettingsIcon /> Plugin configuration
                    </Typography>
                    <br />
                    {this.state.configLoading ? (
                        <React.Fragment>
                            <Skeleton variant="text" height={80} />
                            <Skeleton variant="text" height={40} />
                            <Skeleton variant="text" height={40} />
                            <Skeleton variant="text" height={40} />
                            <Skeleton variant="text" height={40} />
                            <Skeleton variant="text" height={40} />
                            <Skeleton variant="text" height={40} />
                            <Skeleton variant="text" height={40} />
                            <Skeleton variant="text" height={40} />
                        </React.Fragment>
                    ) : this.state.configError ? (
                        <React.Fragment>
                            <LoadingError />
                        </React.Fragment>
                    ) : (
                        <React.Fragment>
                            {' '}
                            <div className="alert alert-info">
                                <ErrorOutlineIcon /> Those settings takes effects immediately when changed.
                            </div>
                            <Grid container spacing={3}>
                                <Grid item xs={12}>
                                    <TooltipHelp title={'Downloads all the external images and embed them as "data-uri" in the cleaned HTML.'} placement="right">
                                        <FormControlLabel control={<Switch checked={this.state.config.embedExternalImages} onChange={() => this.controller!.handleChange('embedExternalImages')} color="primary" />} label="Embed external images" />
                                    </TooltipHelp>

                                    <TooltipHelp title={'Removes all empty tags from the pasted source to clean up the code.'} placement="right">
                                        <FormControlLabel control={<Switch checked={this.state.config.removeEmptyTags} onChange={() => this.controller!.handleChange('removeEmptyTags')} value="checkedB" color="primary" />} label="Remove empty tags" />
                                    </TooltipHelp>

                                    <TooltipHelp title={'Removes all span tags from pasted source.'} placement="right">
                                        <FormControlLabel control={<Switch checked={this.state.config.removeSpanTags} onChange={() => this.controller!.handleChange('removeSpanTags')} value="checkedB" color="primary" />} label="Remove span tags" />
                                    </TooltipHelp>

                                    <TooltipHelp title={'Removes all class names to avoid any side effect when the cleaned text is shown in a web page.'} placement="right">
                                        <FormControlLabel
                                            control={<Switch checked={this.state.config.removeClassNames} onChange={() => this.controller!.handleChange('removeClassNames')} value="checkedB" color="primary" />}
                                            label="Remove class names"
                                        />
                                    </TooltipHelp>

                                    <TooltipHelp title={'Removes all iframes from the pasted source.'} placement="right">
                                        <FormControlLabel control={<Switch checked={this.state.config.removeIframes} onChange={() => this.controller!.handleChange('removeIframes')} value="checkedB" color="primary" />} label="Remove iframes" />
                                    </TooltipHelp>

                                    <TooltipHelp title={'Removes all attributes from the HTML tags, except for the most basic ones like "style".'} placement="right">
                                        <FormControlLabel
                                            control={<Switch checked={this.state.config.removeTagAttributes} onChange={() => this.controller!.handleChange('removeTagAttributes')} value="checkedB" color="primary" />}
                                            label="Remove tag attributes"
                                        />
                                    </TooltipHelp>
                                </Grid>
                            </Grid>
                        </React.Fragment>
                    )}
                </Paper>
            </React.Fragment>
        )
    }
}

export default PluginConfig
