import React from 'react'

import { FormControlLabel, Switch, Grid, Paper, Typography } from '@material-ui/core'
import SettingsIcon from '@material-ui/icons/Settings'
import ErrorOutlineIcon from '@material-ui/icons/ErrorOutline'

import TooltipHelp from '../../../components/TooltipHelp'

import './PluginConfig.less'

export interface PluginConfigProps {}
export interface PluginConfigState {
    embedExternalImages: boolean
    removeEmptyTags: boolean
    removeSpanTags: boolean
    removeClassNames: boolean
    removeIframes: boolean
    removeTagAttributes: boolean
}

class PluginConfig extends React.Component<PluginConfigProps, PluginConfigState> {
    constructor(props: PluginConfigProps) {
        super(props)
        this.state = {
            embedExternalImages: true,
            removeEmptyTags: true,
            removeSpanTags: true,
            removeClassNames: true,
            removeIframes: true,
            removeTagAttributes: true,
        }
    }

    handleChange(config: string, value: boolean) {
        switch (config) {
            case 'embedExternalImages':
                this.setState({ embedExternalImages: value })
                break
            case 'removeEmptyTags':
                this.setState({ removeEmptyTags: value })
                break
            case 'removeSpanTags':
                this.setState({ removeSpanTags: value })
                break
            case 'removeClassNames':
                this.setState({ removeClassNames: value })
                break
            case 'removeIframes':
                this.setState({ removeIframes: value })
                break
            case 'removeTagAttributes':
                this.setState({ removeTagAttributes: value })
                break
        }

        // save backend
    }

    render() {
        return (
            <React.Fragment>
                <Paper className="paper">
                    <Typography variant="h2" className="override-h2" component="h2">
                        <SettingsIcon /> Plugin configuration
                    </Typography>
                    <br />
                    <div className="alert alert-info">
                        <ErrorOutlineIcon /> Those settings takes effects immediately when changed.
                    </div>
                    <Grid container spacing={3}>
                        <Grid item xs={12}>
                            <TooltipHelp title={'Downloads all the external images and embed them as "data-uri" in the cleaned HTML.'} placement="right">
                                <FormControlLabel
                                    control={<Switch checked={this.state.embedExternalImages} onChange={() => this.handleChange('embedExternalImages', !this.state.embedExternalImages)} color="primary" />}
                                    label="Embed external images"
                                />
                            </TooltipHelp>

                            <TooltipHelp title={'Removes all empty tags from the pasted source to clean up the code.'} placement="right">
                                <FormControlLabel
                                    control={<Switch checked={this.state.removeEmptyTags} onChange={() => this.handleChange('removeEmptyTags', !this.state.removeEmptyTags)} value="checkedB" color="primary" />}
                                    label="Remove empty tags"
                                />
                            </TooltipHelp>

                            <TooltipHelp title={'Removes all span tags from pasted source.'} placement="right">
                                <FormControlLabel control={<Switch checked={this.state.removeSpanTags} onChange={() => this.handleChange('removeSpanTags', !this.state.removeSpanTags)} value="checkedB" color="primary" />} label="Remove span tags" />
                            </TooltipHelp>

                            <TooltipHelp title={'Removes all class names to avoid any side effect when the cleaned text is shown in a web page.'} placement="right">
                                <FormControlLabel
                                    control={<Switch checked={this.state.removeClassNames} onChange={() => this.handleChange('removeClassNames', !this.state.removeClassNames)} value="checkedB" color="primary" />}
                                    label="Remove class names"
                                />
                            </TooltipHelp>

                            <TooltipHelp title={'Removes all iframes from the pasted source.'} placement="right">
                                <FormControlLabel control={<Switch checked={this.state.removeIframes} onChange={() => this.handleChange('removeIframes', !this.state.removeIframes)} value="checkedB" color="primary" />} label="Remove iframes" />
                            </TooltipHelp>

                            <TooltipHelp title={'Removes all attributes from the HTML tags, except for the most basic ones like "style".'} placement="right">
                                <FormControlLabel
                                    control={<Switch checked={this.state.removeTagAttributes} onChange={() => this.handleChange('removeTagAttributes', !this.state.removeTagAttributes)} value="checkedB" color="primary" />}
                                    label="Remove tag attributes"
                                />
                            </TooltipHelp>
                        </Grid>
                    </Grid>
                </Paper>
            </React.Fragment>
        )
    }
}

export default PluginConfig
